using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using System.Xml;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace QuestParser
{
    public partial class Form1 : Form
    {
        private string Author;
        private bool AutoParse;
        public Form1()
        {
            InitializeComponent();

            Author = "";
            AutoParse = false;

            string[] args = Environment.GetCommandLineArgs();
            bool quest = false;
            bool author = false;
            
            foreach (string s in args)
            {
                if (s.StartsWith("-"))
                {
                    quest = s.Contains("-quest");
                    author = s.Contains("-author");
                }
                else
                {
                    if (quest)
                    {
                        txtFile.Text = s;
                        AutoParse = true;
                    }
                    if (author)
                        Author = s;
                }
            }
        }

        private void btnParse_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtFile.Text))
                return;

            string url = "http://census.daybreakgames.com/xml/get/eq2/quest?name=";
            XmlReader reader = XmlReader.Create(url + txtFile.Text);
            Quest quest = null;
            TaskGroup task = null;
            while (reader.Read())
            {
                // Every result from census.daybreakgames.com will have a quest_list element, even if no quests are returned
                // we will check the "returned" attribute to ensure we actually have a quest to parse.
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "quest_list")
                {
                    // If returned = 0 then we have no quest
                    if (reader.GetAttribute("returned") == "0")
                    {
                        reader.Close();
                        MessageBox.Show("Could not find " + txtFile.Text + " on census.daybreakgames.com", "Quest not found!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                // Get the quest data from the quest element
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "quest")
                {
                    quest = new Quest();
                    quest.Category = reader.GetAttribute("category");
                    quest.Name = reader.GetAttribute("name");
                    quest.Level = byte.Parse(reader.GetAttribute("level"));
                    quest.ScaleWithLevel = reader.GetAttribute("scales_with_level") == "1";
                    quest.IsTradeskill = reader.GetAttribute("is_tradeskill") == "1";
                    quest.DaybreakCRC = Int64.Parse(reader.GetAttribute("crc"));
                    quest.CompletionText = reader.GetAttribute("completion_text");
                    quest.IsShareable = reader.GetAttribute("shareable") == "1";
                    quest.StarterText = reader.GetAttribute("starter_text");
                    quest.CompleteShareable = reader.GetAttribute("complete_shareable") == "1";
                    quest.Tier = byte.Parse(reader.GetAttribute("tier"));
                    quest.Repeatable = reader.GetAttribute("repeatable") == "1";
                    quest.DaybreakID = Int64.Parse(reader.GetAttribute("id"));
                }

                // Create a new task group for every stage element and add it to the quest
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "stage")
                {
                    task = new TaskGroup();
                    quest.TaskGroups.Add(task);
                }

                // Create a step and fill in the information for every branch element and add it to the current task group
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "branch")
                {
                    Step s = new Step();
                    s.quantityMin = int.Parse(reader.GetAttribute("quota_min"));
                    s.description = reader.GetAttribute("description");
                    s.icon = UInt16.Parse(reader.GetAttribute("icon_id"));
                    s.quantityMax = int.Parse(reader.GetAttribute("quota_max"));
                    s.completedDescription = reader.GetAttribute("completed_text");

                    task.steps.Add(s);
                }
                
                // Get the completed description for the task group
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "completion_text")
                {
                    reader.Read();
                    task.completedDescription = reader.Value;
                }

                // Get the description for the task group
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "starter_text")
                {
                    reader.Read();
                    task.description = reader.Value;
                }

                // We only want to parse one quest so break the read loop at the first end element for a quest
                if (reader.NodeType == XmlNodeType.EndElement && reader.Name == "quest")
                    break;
            }
            reader.Close();

            foreach (TaskGroup t in quest.TaskGroups)
            {
                foreach (Step s in t.steps)
                {
                    DialogStepType stepType = new DialogStepType(s.description);
                    stepType.ShowDialog();
                    s.Type = stepType.Type;
                }
            }

            GenerateLuaScript(quest);
        }

        private void GenerateLuaScript(Quest quest)
        {
            string file = quest.Name.Replace(" ", "_") + ".lua";
            file = Regex.Replace(file, @"[^\w\.@-]", "", RegexOptions.None, TimeSpan.FromSeconds(1.5));
            file = file.ToLower();

            StreamWriter writer = new StreamWriter(Application.StartupPath + "/" + file);

            // Build the lua quest header
            writer.WriteLine("--[[");
            writer.WriteLine("\tScript Name\t\t:\t" + file);
            writer.WriteLine("\tScript Purpose\t:\tHandles the quest, \"" + quest.Name + "\"");
            writer.WriteLine("\tScript Author\t:\t" + (string.IsNullOrEmpty(Author) ? "QuestParser (Replace this)" : Author));
            writer.WriteLine("\tScript Date\t\t:\t" + DateTime.Now.Date.ToString("d"));
            writer.WriteLine("\tScript Notes\t:\tAuto generated with QuestParser.");
            writer.WriteLine();
            writer.WriteLine("\tZone\t\t\t:\t" + quest.Category);
            writer.WriteLine("\tQuest Giver\t\t:\t");
            writer.WriteLine("\tPreceded by\t\t:\tNone");
            writer.WriteLine("\tFollowed by\t\t:\tNone");
            writer.WriteLine("--]]");
            writer.WriteLine();
            writer.WriteLine();

            UInt16 step_count = 1;
            Step prev_step = null;
            for (int i = 0; i < quest.TaskGroups.Count; i++)
            {
                for (int y = 0; y < quest.TaskGroups[i].steps.Count; y++)
                {
                    if (i == 0 && y == 0)
                    {
                        writer.WriteLine("function Init(Quest)");
                        if (quest.IsTradeskill)
                            writer.WriteLine("\tSetQuestFeatherColor(Quest, 2)");

                        if (quest.Repeatable)
                        {
                            // Don't want to set the feather again if it is a repeatable tradeskill quest
                            if (!quest.IsTradeskill)
                                writer.WriteLine("\tSetQuestFeatherColor(Quest, 3)");

                            writer.WriteLine("\tSetQuestRepeatable(Quest)");
                        }
                    }
                    else
                        writer.WriteLine("function Step" + (step_count - 1) + "Complete(Quest, QuestGiver, Player)");


                    Step step = quest.TaskGroups[i].steps[y];
                    string step_type = "";
                    switch (step.Type)
                    {
                        case StepType.Chat:
                            step_type = "AddQuestStepChat";
                            break;
                        case StepType.Craft:
                            step_type = "AddQuestStepCraft";
                            break;                        
                        case StepType.Harvest:
                            step_type = "AddQuestStepHarvest";
                            break;
                        case StepType.Kill:
                            step_type = "AddQuestStepKill";
                            break;
                        case StepType.Location:
                            step_type = "AddQuestStepLocation";
                            break;
                        case StepType.ObtainItem:
                            step_type = "AddQuestStepObtainItem";
                            break;
                        case StepType.Spell:
                            step_type = "AddQuestStepSpell";
                            break;
                        case StepType.Generic:
                        default:
                            step_type = "AddQuestStep";
                            break;
                    }

                    if (step_count > 1)
                    {
                        writer.WriteLine("\tUpdateQuestStepDescription(Quest, " + (step_count - 1) + ", \"" + prev_step.completedDescription + "\")");
                        // If first step of the taskgroup but not the first taskgroup
                        if (i > 0 && y == 0)
                            writer.WriteLine("\tUpdateQuestTaskGroupDescription(Quest, " + i + ", " + quest.TaskGroups[i - 1].completedDescription + ")");

                        writer.WriteLine();
                    }

                    // AddQuestStep(Quest, Step, Description, Quantity, Percentage, TaskGroup, Icon)
                    writer.WriteLine("\t" + step_type + "(Quest, " + step_count + ", \"" + step.description + "\", " + step.quantityMax + ", " + ((step.Type == StepType.Chat || step.Type == StepType.Location) ? "" : "100, ") + quest.TaskGroups[i].description + ", " + step.icon + (step_type == "AddQuestStep" ? ")" : ", --[[ ID's --]])"));

                    string complete_function;
                    if (y + 1 == quest.TaskGroups[i].steps.Count && i + 1 == quest.TaskGroups.Count)
                        complete_function = "QuestComplete";
                    else
                        complete_function = "Step" + step_count + "Complete";

                    writer.WriteLine("\tAddQuestStepCompleteAction(Quest, " + step_count + ", \"" + complete_function + "\")");

                    writer.WriteLine("end");
                    writer.WriteLine();
                    if (i == 0 && y == 0)
                    {
                        writer.WriteLine("function Accepted(Quest, QuestGiver, Player)");
                        writer.WriteLine("\t-- Add dialog here for when the quest is accepted");
                        writer.WriteLine("end");
                        writer.WriteLine();

                        writer.WriteLine("function Declined(Quest, QuestGiver, Player)");
                        writer.WriteLine("\t-- Add dialog here for when the quest is declined");
                        writer.WriteLine("end");
                        writer.WriteLine();

                        writer.WriteLine("function Deleted(Quest, QuestGiver, Player)");
                        writer.WriteLine("\t-- Remove any quest specific items here when the quest is deleted");
                        writer.WriteLine("end");
                        writer.WriteLine();
                    }

                    step_count++;
                    prev_step = step;
                }
            }

            // remove 1 from step_count so it is exact
            step_count--;

            // Add the complete function
            writer.WriteLine("function QuestComplete(Quest, QuestGiver, Player)");

            writer.WriteLine("\t-- The following UpdateQuestStepDescription and UpdateTaskGroupDescription are not needed, parser adds them for completion in case stuff needs to be moved around");
            writer.WriteLine("\tUpdateQuestStepDescription(Quest, " + step_count + ", \"" + prev_step.completedDescription + "\")");
            writer.WriteLine("\tUpdateQuestTaskGroupDescription(Quest, " + quest.TaskGroups.Count + ", " + quest.TaskGroups[quest.TaskGroups.Count - 1].completedDescription + ")");
            writer.WriteLine();

            writer.WriteLine("\tUpdateQuestDescription(Quest, \"" + quest.CompletionText + "\")");
            writer.WriteLine("\tGiveQuestReward(Quest, Player)");
            writer.WriteLine("end");


            writer.WriteLine();
            writer.WriteLine("function Reload(Quest, QuestGiver, Player, Step)");
            for (int i = 1; i <= step_count; i++)
            {
                if (i == 1)
                    writer.Write("\tif ");
                else
                    writer.Write("\telseif ");
                
                writer.WriteLine("Step == " + i + " then");
                writer.Write("\t\t");
                if (i == step_count)
                    writer.Write("QuestComplete");
                else
                    writer.Write("Step" + i + "Complete");

                writer.WriteLine("(Quest, QuestGiver, Player)");

                if (i == step_count)
                    writer.WriteLine("\tend");
            }
            writer.WriteLine("end");
            writer.Close();

            if (MessageBox.Show(file + " has been created, would you like to open it?", "Done!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ProcessStartInfo sInfo = new ProcessStartInfo(Application.StartupPath + "/" + file);
                Process.Start(sInfo);
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            if (AutoParse)
                btnParse_Click(null, null);
        }
    }



    public enum StepType
    {
        Chat,
        Craft,
        Generic,
        Harvest,
        Kill,
        Location,
        ObtainItem,
        Spell
    };

    public class Quest
    {
        public string Category;
        public string Name;
        public byte Level;
        public bool ScaleWithLevel;
        public bool IsTradeskill;
        public string CompletionText;
        public Int64 DaybreakCRC;
        public bool IsShareable;
        public string StarterText;
        public bool CompleteShareable;
        public byte Tier;
        public bool Repeatable;
        public Int64 DaybreakID;

        public List<TaskGroup> TaskGroups;
        public Quest()
        {
            TaskGroups = new List<TaskGroup>();
        }
    };

    public class TaskGroup
    {
        public string description;
        public string completedDescription;
        public List<Step> steps;

        public TaskGroup()
        {
            steps = new List<Step>();
        }
    };

    public class Step
    {
        public string description;
        public string completedDescription;
        public int quantityMin;
        public int quantityMax;
        public UInt16 icon;
        public string iconName;
        public StepType Type;
    };
}

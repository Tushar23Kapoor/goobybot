using SharpConfig;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace goobybot.Core.Config
{
    static class Config
    {
        /// <summary>
        /// Static constructor for initialization
        /// </summary>
        static Config()
        {
            if (!File.Exists("Config.txt"))
                File.WriteAllText("Config.txt", "");
            var config = Configuration.LoadFromFile("Config.txt");

            // get sections
            var general = config["General"];

            // GENERAL CONFIG DATA
            CreateEntryIfNull(general, "AUTH_TOKEN", "", EntryType.STRING, "The auth token for the discord bot.");
            BOT_TOKEN = general["AUTH_TOKEN"].StringValue;

            CreateEntryIfNull(general, "ADMIN_DISC_IDS", "", EntryType.STRING, "Administrator discord id's, seperated by commas.");
            ADMINISTRATOR_DISC_IDS = general["ADMIN_DISC_IDS"].StringValue.Replace(" ", "").Split(",", StringSplitOptions.RemoveEmptyEntries).ToArray();

            CreateEntryIfNull(general, "COMMAND_PREFIX", "!", EntryType.STRING, "Command prefix. Single char. Ex: ! or .");
            COMMAND_PREFIX = general["COMMAND_PREFIX"].StringValue;

            // POST READING DATA, UPDATE CONFIG
            config.SaveToFile("Config.txt");
        }

        /// <summary>
        /// Allows you to add a new entry to the config with a default value through code
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <param name="entryType"></param>
        /// <param name="preComment"></param>
        public static void CreateEntryIfNull(Section section, string key, object defaultValue, EntryType entryType, string preComment = "")
        {
            if (string.IsNullOrWhiteSpace(section[key].StringValue))
            {
                // section doesnt exist, create it
                switch (entryType)
                {
                    case EntryType.BOOL:
                        section[key].BoolValue = (bool)defaultValue;
                        break;
                    case EntryType.FLOAT:
                        section[key].FloatValue = (float)defaultValue;
                        break;
                    case EntryType.INT:
                        section[key].IntValue = (int)defaultValue;
                        break;
                    case EntryType.STRING:
                        section[key].StringValue = (string)defaultValue;
                        break;
                }
            }
            if (!string.IsNullOrWhiteSpace(preComment))
                section[key].PreComment = preComment;
        }
        public enum EntryType { BOOL, STRING, INT, FLOAT }

        public static string BOT_TOKEN { get; set; }
        public static string[] ADMINISTRATOR_DISC_IDS { get; set; }
        public static string COMMAND_PREFIX { get; set; }
    }
}
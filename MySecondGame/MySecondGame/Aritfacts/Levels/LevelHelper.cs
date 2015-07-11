using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace MySecondGame.Aritfacts.Levels
{
    public class LevelHelper
    {
        public static List<Level> GetLevels(bool defaultLevel = true)
        {

            System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(LevelCollection));
            System.IO.StreamReader file = new System.IO.StreamReader(@"Levels.xml");
            LevelCollection levels = (LevelCollection)reader.Deserialize(file);

            List<Level> loadedLevels = new List<Level>();

            foreach (LevelItem l in levels.Items)
            {
                Level lvl = new Level();
                lvl.Name = l.Name;
                lvl.AssetName = l.Content;

                if (l.Default == "True")
                    lvl.Default = true;

                lvl.Exits = new Dictionary<string, Exit>();
                foreach (LevelExit le in l.AllExits.AllExits )
                {
                    Exit e = new Exit();
                    e.destination = le.Room;
                    e.destinationX = le.destinationX;
                    e.destinationY = le.destinationY;

                    lvl.Exits.Add(le.Direction, e);
                }

                loadedLevels.Add(lvl);
            }
            
            return loadedLevels;
        }
    }

    [XmlRoot("Levels")]
    public class LevelCollection
    {
        public LevelCollection() { Items = new List<LevelItem>(); }
        [XmlElement("Level")]
        public List<LevelItem> Items { get; set; }
    }
    public class LevelItem
    {
        [XmlElement("Name")]
        public String Name { get; set; }

        [XmlElement("Content")]
        public String Content { get; set; }

        [XmlElement("Default")]
        public String Default { get; set; }

        [XmlElement("Exits")]
        public LevelExits AllExits { get; set; }

    }

    public class LevelExits
    {
        public LevelExits() { AllExits = new List<LevelExit>(); }
        [XmlElement("Exit")]
        public List<LevelExit> AllExits { get; set; }
    }

    public class LevelExit
    {
        [XmlElement("Direction")]
        public string Direction { get; set; }

        [XmlElement("Room")]
        public string Room { get; set; }

        [XmlElement("X")]
        public int destinationX { get; set; }
        
        [XmlElement("Y")]
        public int destinationY { get; set; }
    }


}

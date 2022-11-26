
using Newtonsoft.Json.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Level_Data.StrategyPattern
{

        public class ItemFactory
        {

            public string type { get; set; }
            public string? color { get; set; }
            public int x { get; set; }
            public int y { get; set; }
            public int? damage { get; set; }
            public JToken JsonItem { get; }

        
        public ItemFactory(JToken jsonItem)
        {

            type = jsonItem["type"].Value<string>();
            x = jsonItem["x"].Value<int>();
            y = jsonItem["y"].Value<int>();

            try
            {
                color = jsonItem["color"].Value<string?>();
            }
            catch (Exception)
            {
                color = "";
            }

            try
            {
                damage = jsonItem["damage"].Value<int?>();
            }
            catch (Exception)
            {
                damage  = 0;
            }
        }
        
        public ItemFactory(XmlNode xmlItem)
        {
            type = xmlItem.LocalName;
            x = Int32.Parse(xmlItem.Attributes["x"].Value);
            y = Int32.Parse(xmlItem.Attributes["y"].Value);

         
                if (xmlItem.Attributes != null && xmlItem.Attributes["color"] != null)
                {
                    color = xmlItem.Attributes["color"].Value;
                }
                
                else
                {
                    color = "";
                }


                if (xmlItem.Attributes != null && xmlItem.Attributes["damage"] != null)
                {
                    damage = Int32.Parse(xmlItem.Attributes["damage"].Value);
                }

                else
                {
                    damage = 0;
                }

        }


        public Iitem ProduceItems()
            {
                if ("sankara stone".Equals(type))
                {
                    return new SankaraStoneItem(type, color, x, y, damage);
                }

                else if ("boobytrap".Equals(type))
                {
                    return new BoobyTrapItem(type, color, x, y, damage);
                }

                else if ("disappearing boobytrap".Equals(type))
                {
                    return new DisapearingBoobyTrapItem(type, color, x, y, damage);
                }

                else if ("pressure plate".Equals(type))
                {
                    return new PressurePlateItem(type, color, x, y, damage);
                }

                else if ("sankara stone".Equals(type))
                {
                    return new SankaraStoneItem(type, color, x, y, damage);
                }

                else if ("key".Equals(type))
                {
                    return new KeyItem(type, color, x, y, damage);
                }
                return null;
            }
        }
    }



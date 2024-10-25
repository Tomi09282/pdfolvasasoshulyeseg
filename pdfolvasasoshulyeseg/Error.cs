using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pdfolvasasoshulyeseg
{



    public class Error
    {
        public string Id { get; set; }                  // Azonosító
        public string ErrorName { get; set; }           // Hibaneve
        public string Synopsis { get; set; }            // Rövid leírás
        public string Description { get; set; }         // Leírás
        public string SeeAlso { get; set; }             // További információk linkje
        public string Solution { get; set; }            // Javasolt megoldás
        public string RiskFactor { get; set; }          // Kockázati faktor
        public string CVSSv3BaseScore { get; set; }     // CVSS v3.0 alap pontszám
        public string CVSSv3TemporalScore { get; set; } // CVSS v3.0 időbeli pontszám
        public string CVSSBaseScore { get; set; }       // CVSS alap pontszám
        public string CVSSTemporalScore { get; set; }   // CVSS időbeli pontszám
        public string References { get; set; }          // Referenciák
        public string PluginInformation { get; set; }   // Plugin információk
        public string PluginOutput { get; set; }        // Plugin kimenet

        // Üres konstruktor
        public Error()
        {
            // Alapértelmezett értékek beállítása, ha nincs adat
            Id = "No data available";
            ErrorName = "No data available";
            Synopsis = "No data available";
            Description = "No data available";
            SeeAlso = "No data available";
            Solution = "No data available";
            RiskFactor = "No data available";
            CVSSv3BaseScore = "No data available";
            CVSSv3TemporalScore = "No data available";
            CVSSBaseScore = "No data available";
            CVSSTemporalScore = "No data available";
            References = "No data available";
            PluginInformation = "No data available";
            PluginOutput = "No data available";
        }
    }
    internal class CriticalError
    {
        public string Id { get; set; }
        public string ErrorName { get; set; }
        public string Synopsis { get; set; }
        public string Description { get; set; }
        public string SeeAlso { get; set; }
        public string Solution { get; set; }
        public string RiskFactor { get; set; }
        public string CVSSv3BaseScore { get; set; }
        public string CVSSv3TemporalScore { get; set; }
        public string CVSSBaseScore { get; set; }
        public string CVSSTemporalScore { get; set; }
        public string References { get; set; }
        public string PluginInformation { get; set; }
        public string PluginOutput { get; set; }
    }

    internal class MediumError
    {
        public string Id { get; set; }
        public string ErrorName { get; set; }
        public string Synopsis { get; set; }
        public string Description { get; set; }
        public string SeeAlso { get; set; }
        public string Solution { get; set; }
        public string RiskFactor { get; set; }
        public string CVSSv3BaseScore { get; set; }
        public string CVSSBaseScore { get; set; }
        public string PluginInformation { get; set; }
        public string PluginOutput { get; set; }
    }
}
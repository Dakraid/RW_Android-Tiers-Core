using RimWorld;
using Verse;

namespace MOARANDROIDS
{
    public class Alert_UnsecurisedClients : Alert
    {
        public Alert_UnsecurisedClients()
        {
            defaultPriority = AlertPriority.High;
        }


        public override AlertReport GetReport()
        {
            if (Settings.disableSkyMindSecurityStuff)
                return false;

            var nbSecurisedSlot = Utils.GCATPP.getNbSlotSecurisedAvailable();
            var nbClient = Utils.GCATPP.getNbThingsConnected();
            var nbUnsecurised = nbClient - nbSecurisedSlot;

            if (nbUnsecurised <= 0) return false;
            
            defaultLabel = "ATPP_AlertUnsecurisedClients".Translate(nbUnsecurised);
            defaultExplanation = "ATPP_AlertUnsecurisedClientsDesc".Translate(nbUnsecurised);
            return true;
        }
    }
}
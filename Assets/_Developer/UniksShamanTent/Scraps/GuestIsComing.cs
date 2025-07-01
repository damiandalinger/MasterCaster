using UnityEngine;
using System.Collections.Generic;

namespace ProjectCeros
{
    public class GuestIsComing : MonoBehaviour
    {
        public GuestDatabaseSO AllGuests;

        public void CheckAccepted()
        {
            foreach (var guest in AllGuests.AllGuests)
            {
                if (guest.hasAccepted)

                {
                    /*
                    Guest comes

                    Player receives positive E - Mail
                    Player receives a Modifier
                    Guest appears in the Podcast
                    New topic gets unlocked(Talk to Guest)

                    Player talks about guest, Guest Dialogue plays, mild modifier
                    
                    Player chooses regular topic
                    Correct topic: Bonus and positive dialogue
                    Incorrect topic: Low bonus and negative dialogue
                    Modifier gets reset after

                    Guest does not come
                    Player receives negative E - Mail 
                    */

                }

            }


        }


    }



}
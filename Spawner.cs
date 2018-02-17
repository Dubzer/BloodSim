/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodSim
{
   *public class Spawner
    {
        public List<Bacterium> enemiesList;
        public int timeBeforeNextWave;
        public int wavesCount;
        public int enemiesCount;
        public bool alreadyActivated = false;
        public bool alreadyDone = false;

        public event Action AllWaveIsDead;
        public event Action AllWavesAreDone;

        public void Activate(int wavesCount, int timeBeforeNextWave, int enemiesCount)
        {
            if (!alreadyActivated)
            {
                alreadyActivated = true;

                this.wavesCount = wavesCount;
                this.timeBeforeNextWave = timeBeforeNextWave;
                this.enemiesCount = enemiesCount;

                for (int i = 0; i < enemiesCount; i++)
                {
                    enemiesList.Add(new Bacterium());
                }
            }
        }

        public void Activate()
        {
            Activate(1, 10, 5);
        }

        public void AWISD_Reaction()
        {
            alreadyActivated = false;
        }

        public void AWAD_Reaction()
        {
            alreadyDone = true;
        }


    }
}*/

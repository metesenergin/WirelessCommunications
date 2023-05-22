using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumSharing
{
    public static class BlockChain
    {
        public static string [] Mine(string[] List,int seed)
        {

                int sizeofpool=List.Length;
                string[] shuffledlist = new string[sizeofpool];
                var indicesList = new List<int>(sizeofpool);

                for (int i = 0; i < sizeofpool; i++)
                    indicesList.Add(i);

                var random = new Random(seed);
                var shuffled = new List<int>(sizeofpool);
                for (int i = 0; i < sizeofpool; i++)
                {
                    var randomElementInList = random.Next(0, indicesList.Count - 1);
                    shuffled.Add(indicesList[randomElementInList]);
                    indicesList.Remove(indicesList[randomElementInList]);
                }

                for (int i = 0; i < sizeofpool; i++)
                {
                    shuffledlist[i] = List[shuffled[i]];
                }
                return shuffledlist;
            
        }

        public static string[] RandomStatus(string[] List, int seed)
        {

            int sizeofpool = List.Length;
            string[] shuffledlist = new string[sizeofpool];
            var indicesList = new List<int>(sizeofpool);

            for (int i = 0; i < sizeofpool; i++)
                indicesList.Add(i);

            var random = new Random(seed);
            var shuffled = new List<int>(sizeofpool);
            for (int i = 0; i < sizeofpool; i++)
            {
                var randomElementInList = random.Next(0, indicesList.Count - 1);
                shuffled.Add(indicesList[randomElementInList]);
                indicesList.Remove(indicesList[randomElementInList]);
            }

            for (int i = 0; i < sizeofpool; i++)
            {
                shuffledlist[i] = List[shuffled[i]];
            }
            return shuffledlist;

        }

        public static string[] RandomIsBidding(string[] List, int seed)
        {

            int sizeofpool = List.Length;
            
            string[] shuffledlist = new string[sizeofpool];
            var indicesList = new List<int>(sizeofpool);

            for (int i = 0; i < sizeofpool; i++)
                indicesList.Add(i);

            var random = new Random(seed);
            var shuffled = new List<int>(sizeofpool);
            for (int i = 0; i < sizeofpool; i++)
            {
                var randomElementInList = random.Next(0, indicesList.Count - 1);
                shuffled.Add(indicesList[randomElementInList]);
                indicesList.Remove(indicesList[randomElementInList]);
            }

            for (int i = 0; i < sizeofpool; i++)
            {
                shuffledlist[i] = List[shuffled[i]];
            }
            return shuffledlist;

        }
    }
}

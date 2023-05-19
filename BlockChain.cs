using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpectrumSharing
{
    internal class BlockChain
    {
        public static string [] Mine(string[]poolList,int seed)
        {

                int sizeofpool=poolList.Length;
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
                    shuffledlist[i] = poolList[shuffled[i]];
                }
                return shuffledlist;
            
        }
    }
}

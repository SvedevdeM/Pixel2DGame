using System.Collections.Generic;
using UnityEngine;


namespace JabberwockyWorld.AnimationSystem.Scripts
{
    [CreateAssetMenu(fileName = "SimpleAnimation", menuName = "Animation System/Simple Animation", order = 400)]
    public class SimpleAnimationNode : AnimationNode<CommonCel>
    {
        public void Initialize(string name, List<AnimationCellsData> cells, Controll controll, List<AnimationDriverDIctData> driverDictionary, bool isSTartingNode)
        {
            int i = 0;
            Name = name;
            cels = new CommonCel[cells.Count];
            foreach (AnimationCellsData cell in cells)
            {
                cels[i] = new CommonCel();
                cels[i].ChangeSprite(cell.Sprite);
                i++;
            }
            i = 0;
            foreach (AnimationDriverDIctData driverD in driverDictionary)
            {
                if(driverD.Controll != null)
                {
                    drivers.values.Add(driverD.Key);
                    drivers.keys.Add(driverD.Controll);
                }
            }
            IsStartingNode = isSTartingNode;
            controlDriverNew.ChangeControl(controll);
        }
    }
}
using System.Collections.Generic;

namespace Global.SaveSystem.SavableObjects
{
    public class OpenSkills : ISavable
    {
        public List<SkillWithLevel> OpenedSkills = new();
        

    }
}
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using TEngine;

namespace GameLogic
{
    public partial class AbilityTemplate
    {
        public static List<AbilityTemplate> Templates { get; private set; }

        public static async UniTaskVoid LoadTemplatesFromJson()
        {
            TextAsset jsonAsset = await GameModule.Resource.LoadAssetAsync<TextAsset>("AbilityTemplates");
            AbilityTemplate.Templates = Utility.Json.ToObject<List<AbilityTemplate>>(jsonAsset.text);
            if (Templates != null)
            {
                foreach (var template in Templates)
                {
                    template.OnDeserialized();
                }
            }
        }
        
        public static AbilityTemplate GetTemplate(string abilityId)
        {
            return Templates?.Find(t => t.Id == abilityId);
        }
    }
}

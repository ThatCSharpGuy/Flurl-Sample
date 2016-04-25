using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Flurl_Sample
{
    public class Pokemon
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [JsonProperty("base_experience")]
        public int BaseExperience { get; set; }
        public int Height { get; set; }


        [JsonProperty("is_default")]
        public bool IsDefault { get; set; }
        public int Order { get; set; }
        public int Weight { get; set; }
        public List<PokemonAbility> Abilities { get; set; }
    }

    public class PokemonAbility
    {
        [JsonProperty("is_hidden")]
        public bool IsHidden { get; set; }
        public int Slot { get; set; }
    }
}

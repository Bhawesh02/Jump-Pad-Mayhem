using System.Collections.Generic;
using UnityEngine;

namespace KWCreative
{
    public class CreativeConfig : GenericConfig<CreativeConfig>
    {
        [SerializeField] private List<VariationData> m_variationDatas;

        public VariationType currentVariation;
        
        public VariationData CurrentVariationData =>
            m_variationDatas.Find(data => data.variationType == currentVariation);

    }
}


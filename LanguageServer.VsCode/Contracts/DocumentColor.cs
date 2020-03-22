using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace LanguageServer.VsCode.Contracts
{
    [JsonObject]
    public class ColorInformation
    {
        /// <summary>
        /// The range in the document where this color appears.
        /// </summary>
        public Range Range { get; set; }

        /// <summary>
        /// The actual color value for this color range.
        /// </summary>
        public Color Color { get; set; }
    }

    /// <summary>
    /// Represents a color in RGBA space.
    /// </summary>
    [JsonObject]
    public class Color
    {
        public Color(float red, float green, float blue, float alpha)
        {
            Red = red;
            Green = green;
            Blue = blue;
            Alpha = alpha;
        }

        public float Red { get; }

        public float Green { get; }

        public float Blue { get; }

        public float Alpha { get; }
    }

    [JsonObject]
    public class ColorPresentation
    {
        /// <summary>
        /// The label of this color presentation. It will be shown on the color
        /// picker header. By default this is also the text that is inserted when selecting
        /// this color presentation.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// An <see cref="TextEdit"/> which is applied to a document when selecting
        /// this presentation for the color.  When falsy(<c>null</c>) the <see cref="Label"/>
        /// is used.
        /// </summary>
        public TextEdit TextEdit { get; set; }

        /// <summary>
        /// An optional array of additional <see cref="TextEdit"/>s that are applied when
        /// selecting this color presentation.
        /// Edits must not overlap with the main <see cref="TextEdit"/> nor with themselves.
        /// </summary>
        public ICollection<TextEdit> AdditionalTextEdits { get; set; }
    }

}

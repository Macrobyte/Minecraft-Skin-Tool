using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomAttributes
{
    public class ReadOnlyAttribute : PropertyAttribute
    {
        // This is just a marker class
    }

    public class DividerAttribute : PropertyAttribute
    {
        public float thickness;
        public Color color;

        public DividerAttribute(float thickness, float r, float g, float b, float a)
        {
            this.thickness = thickness;
            this.color = new Color(r, g, b, a);
        }
    }

    public class CategoryAttribute : PropertyAttribute
    {
        public string label;
        public TextAnchor textAnchor;

        public CategoryAttribute(string categoryLabel, TextAnchor textAnchor)
        {
            this.label = categoryLabel;
            this.textAnchor = textAnchor;

        }
    }
}
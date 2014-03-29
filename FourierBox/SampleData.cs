namespace FourierBox
{
    using System.Collections.Generic;
    using OxyPlot;

    public class SampleData
    {
        public SampleData(IFunction function, string name)
        {
            Function = function;
            Name = name;
        }
        public IFunction Function { get; private set; }
        public string Name { get; private set; }
    }
}
using System;
using NRedberry.Core.Indices;
using NRedberry.Core.Tensors;

namespace NRedberry.Core.Contexts
{
    public sealed class ContextSettings
    {
        private OutputFormat redberry;
        private string v;

        public ContextSettings(OutputFormat redberry, string v)
        {
            this.redberry = redberry;
            this.v = v;
        }

        public ContextSettings()
        {
        }

        public NRedberry.Core.Parser getParser()
        {
            throw new NotImplementedException();
        }

        public IndexConverterManager getConverterManager()
        {
            throw new NotImplementedException();
        }

        public int getNameManagerSeed()
        {
            throw new NotImplementedException();
        }

        public string getKronecker()
        {
            throw new NotImplementedException();
        }

        public string getMetricName()
        {
            throw new NotImplementedException();
        }

        public OutputFormat getDefaultOutputFormat()
        {
            throw new NotImplementedException();
        }

        public IndexType[] MetricTypes { get; set; }

        public void setMetricName(string s)
        {
            throw new NotImplementedException();
        }

        public void addMetricIndexType(IndexType indexType)
        {
            throw new NotImplementedException();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECR_System_v2.Loaders.Listeners
{
    public interface DataEntryListener
    {
        void SingleEntry(Type mType,String json);
        void MultipleEntry(Type mType, String json);
        void Deletion(Type mType, String json);
    }
}

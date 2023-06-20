using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuarkLang.Core.Runtime;

public class Parameter
{
    public readonly string _name;
    public readonly TypeValue _type;

    public Parameter(string name, TypeValue type)
    {
        _name = name;
        _type = type;
    }
}
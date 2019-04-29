using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RhinoPythonForVisualStudio
{
    public static class PythonTemplate
    {
        /// <summary>
        /// The run-function python template string.
        /// </summary>
        public static string RunFunctionTemplate = @"import classes as MOD
import scriptcontext
import rhinoscriptsyntax as rs

def main():
    try:
        name = MOD.CLASSPATHCLASSNAME.DESCRIPTION
    except:
        name = 'CLASSNAME'
    try:
        items = MOD.CLASSPATHCLASSNAME.GetInstances('Select [%s]s to process' % name)
        bRedraw = rs.EnableRedraw(False)
        if not items: return
        customFunction(items, name)
    finally:
        rs.EnableRedraw(bRedraw)

def customFunction(items, name):
    length = len(items)
    for i, item in enumerate(items):
        rs.Prompt('Processing [%s]s %d/%d' % (name, i+1, length))
        scriptcontext.escape_test()
        item.FUNCTIONNAME()


if __name__ == '__main__':
    main()
";
        /// <summary>
        /// The print property python template string.
        /// </summary>
        public static string PrintProertyTemplate = @"import classes as MOD
import scriptcontext
import rhinoscriptsyntax as rs

def main():
    try:
        name = MOD.CLASSPATHCLASSNAME.DESCRIPTION
    except:
        name = 'CLASSNAME'
    try:
        items = MOD.CLASSPATHCLASSNAME.GetInstances('Select [%s]s to process' % name)
        bRedraw = rs.EnableRedraw(False)
        if not items: return
        customFunction(items, name)
    finally:
        rs.EnableRedraw(bRedraw)

def customFunction(items, name):
    length = len(items)
    for i, item in enumerate(items):
        rs.Prompt('Processing [%s]s %d/%d' % (name, i+1, length))
        scriptcontext.escape_test()
        print item.FUNCTIONNAME


if __name__ == '__main__':
    main()
";
    }
}

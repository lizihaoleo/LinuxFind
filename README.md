<h1> Linux Find Command Line </h1>
<h4> This is a experiement project to understand OOP concepts and LL parser </h4>
Support -size, -name, -maxdepth, -writetofile and logical connection words (-and, -or, -not)

    ./find.exe . -name *.txt -or -size +10MB -and -maxdepth 2 -writetofile tmp.txt

* Looking files recursively with max depth = 2, search file with .txt ending OR size greater than 10 Mb, then write the full path of files into tmp.txt


According to the Linux command document, find command should support followings:

    1. options：affect overall operation rather than the processing of a specific file; (-maxdepth)

    2. filter：return a true or false value, depending on the file's attributes; (-size, -name)

    3. actions：have side effects and return a true or false value; (-writetofile)

    4. operators：connect the other arguments and affect when and whether they are evaluated. (-and, -or, -not)
<br>
One main benefit of OOP is the project should be easy for extension feature, in order to adding new option/filter/action, it only need 3 steps:

    1. create a new class inherit optionBase/filterBase/actionBase class

    2. create a new parser inherit parseBase 

    3. regist new parser in ExecutionGenerator
<br>
Also, one of the challenging parts for this project is to write a parser to support logical operator, and I implement a LL parser (recursive parser) in ExecutionGenerator class. 

More LL parser referece in [here](https://zhuanlan.zhihu.com/p/31271879).

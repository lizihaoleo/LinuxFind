namespace LinuxFind.Bases
{
    using System;
    using System.Collections.Generic;

    // Option/Filter/Action的解析器基类
    /// <summary>
    /// Defines the <see cref="OptionParser" />.
    /// </summary>
    public abstract class OptionParser
    {
        // 该解析器所处理的参数名，例如"maxdepth"，"type"，"size"
        /// <summary>
        /// The getName.
        /// </summary>
        /// <returns>The <see cref="String"/>.</returns>
        public abstract String getName();

        // 解析逻辑的实现
        /// <summary>
        /// The parse.
        /// </summary>
        /// <param name="args">The args<see cref="Stack{String}"/>.</param>
        /// <returns>The <see cref="PlanNode"/>.</returns>
        public abstract PlanNode parse(Stack<String> args);
    }
}

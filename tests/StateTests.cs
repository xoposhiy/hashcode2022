using System;
using NUnit.Framework;

namespace bot;

[TestFixture]
public class StateTests
{
    [TestCase("a_an_example")]
    [TestCase("b_better_start_small")]
    [TestCase("c_collaboration")]
    [TestCase("d_dense_schedule")]
    [TestCase("e_exceptional_skills")]
    [TestCase("f_find_great_mentors")]
    public void Test(string name)
    {
        var problem = new ProblemReader().ReadByName(name);
        Console.WriteLine(problem);
    }
}
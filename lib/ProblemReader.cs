using System.Collections.Generic;
using System.IO;

namespace bot;

public class ProblemReader
{
    public Problem ReadByName(string testCaseName)
    {
        var lines = File.ReadAllLines($"../../../../data/{testCaseName}.in.txt");
        return ReadFrom(lines);
    }

    public Problem ReadFrom(string[] lines)
    {
        var people = new List<Person>();
        var projects = new List<Project>();
        var i = 0;
        var nLine = lines[i++].Split();
        var nPeople = nLine[0].ToInt();
        var nProjects = nLine[1].ToInt();
        for (var iPerson = 0; iPerson < nPeople; iPerson++)
        {
            var line = lines[i++].Split(' ');
            var name = line[0];
            var skillsCount = line[1].ToInt();
            var skills = ReadSkills(skillsCount, lines, ref i);
            people.Add(new Person(name, skills));
        }

        for (var iProject = 0; iProject < nProjects; iProject++)
        {
            var line = lines[i++].Split(' ');
            var name = line[0];
            var duration = line[1].ToInt();
            var score = line[2].ToInt();
            var bestBefore = line[3].ToInt();
            var rolesCount = line[4].ToInt();
            var roles = ReadSkills(rolesCount, lines, ref i);
            projects.Add(new Project(name, duration, score, bestBefore, roles));
        }

        return new Problem(people.ToArray(), projects.ToArray());
    }

    private static Skill[] ReadSkills(int skillsCount, string[] lines, ref int i)
    {
        var skills = new Skill[skillsCount];
        for (var iSkill = 0; iSkill < skillsCount; iSkill++)
        {
            var skillLine = lines[i++].Split(' ');

            skills[iSkill] = new Skill(skillLine[0], skillLine[1].ToInt());
        }

        return skills;
    }
}
namespace bot;

public record Skill(string Name, int Level);

public record Person(string Name, Skill[] Skills)
{
    public override string ToString()
    {
        return $"{Name} {Skills.StrJoin("; ")}";
    }
}

public class Project
{
    public int BestBefore;
    public int Duration;

    public string Name;
    public Skill[] Roles;
    public int Score;

    public Project(string name, int duration, int score, int bestBefore, Skill[] roles)
    {
        Name = name;
        Duration = duration;
        Score = score;
        BestBefore = bestBefore;
        Roles = roles;
    }

    public override string ToString()
    {
        return
            $"{nameof(Name)}: {Name}, {nameof(Duration)}: {Duration}, {nameof(Score)}: {Score}, {nameof(BestBefore)}: {BestBefore}, {nameof(Roles)}: {Roles.StrJoin(", ")}";
    }
}

public class Problem
{
    public Person[] People;
    public Project[] Projects;

    public Problem(Person[] people, Project[] projects)
    {
        People = people;
        Projects = projects;
    }

    public override string ToString()
    {
        return People.StrJoin("\n") + "\n\n" + Projects.StrJoin("\n");
    }
}
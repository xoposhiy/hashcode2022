using System.Linq;

namespace bot;

public record Skill(string Name, int Level);

public record Person(string Name, Skill[] Skills)
{
    public override string ToString()
    {
        return $"{Name} {Skills.StrJoin("; ")}";
    }

    public bool HasSkill(Skill role)
    {
        return Skills.Any(s => s.Name == role.Name && s.Level >= role.Level);
    }
}

public class Idea
{
    public int BestBefore;
    public int Duration;

    public string Name;
    public Skill[] Roles;
    public int Score;

    public Idea(string name, int duration, int score, int bestBefore, Skill[] roles)
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
    public Idea[] Ideas;

    public Problem(Person[] people, Idea[] ideas)
    {
        People = people;
        Ideas = ideas;
    }

    public override string ToString()
    {
        return People.StrJoin("\n") + "\n\n" + Ideas.StrJoin("\n");
    }
}
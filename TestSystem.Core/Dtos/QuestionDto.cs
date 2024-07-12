namespace TestSystem.Core.Dtos;

public record QuestionDto(Guid Id, string Text, string Type, double Weight, List<AnswerDto> Answers, List<MatchPairDto> MatchPairs);
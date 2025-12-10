using System.ComponentModel.DataAnnotations;

namespace dotnet.Dto {
public class ChallengeCreateDto : IValidatableObject {

    public int? ChallengeId { get; set; }

    public string? ChallengeName { get; set; }
    public string? ChallengeType { get; set; }


    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!ChallengeId.HasValue)
        {
            if (string.IsNullOrWhiteSpace(ChallengeName))
                yield return new ValidationResult("ChallengeName is required when ChallengeId is not provided.", new[] { nameof(ChallengeName) });

            if (string.IsNullOrWhiteSpace(ChallengeType))
                yield return new ValidationResult("ChallengeType is required when ChallengeId is not provided.", new[] { nameof(ChallengeType) });
        }
    }
}

}

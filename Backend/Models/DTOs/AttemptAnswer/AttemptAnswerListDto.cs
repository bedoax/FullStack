namespace Models.DTOs.AttemptAnswer
{
    public class AttemptAnswerListDto
    {
        // you get the quiz id and user id from requset and pass it to service layerr to repository to get the attempt id then pass it to this dto to get the list of attempt answers for that attempt
        public int Id { get; set; }

        public int QuestionId { get; set; }
        
        public int? SelectedOptionId { get; set; }

        public bool? IsCorrect { get; set; }
    }
}

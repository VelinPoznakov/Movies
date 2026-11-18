namespace Movie.Common;

public static class EntitiesConstants
{
    public static class MovieEntity
    {
        public const int TitleMaxLength = 200;
        public const int TitleMinLength = 1;

        public const int ImdbIdMaxLength = 20;
        
        public const int PosterBlobNameMaxLength = 200;
    }
    
    public static class Genre
    {
        public const int NameMaxLength = 100;
        public const int NameMinLength = 1;
    }
    
    public static class Director
    {
        public const int NameMaxLength = 100;
        public const int NameMinLength = 1;
        
        public const int ProfilePathMaxLength = 200;
        public const int ProfilePathMinLength = 2;
    }

    public static class Comment
    {
        public const int ContentMaxLength = 1000;
        public const int ContentMinLength = 5;
    }

    public static class ProducerCompany
    {
        public const int NameMaxLength = 200;
        public const int NameMinLength = 1;

        public const int LogoPathMaxLength = 200;
        public const int LogoPathMinLength = 2;
    }
}

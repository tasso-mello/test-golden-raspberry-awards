namespace common.raspberry.awards.Utilities
{
    public static class ModelExtensions
    {
        #region Movie
        public static Models.Movie ToModelMovie(this data.raspberry.awards.Entities.Movie user)
        {
            return new Models.Movie
            {
                Id = user.Id,
                Title = user.Title,
                Producers = user.Producers,
                Studio = user.Studio,
                Winner = user.Winner,
                InsertDate = user.InsertDate,
                UpdateDate = user.UpdateDate,
                UserInsert = user.UserInsert,
                UserUpdate = user.UserUpdate
            };
        }

        public static data.raspberry.awards.Entities.Movie ToEntityMovie(this Models.Movie user)
        {
            return new data.raspberry.awards.Entities.Movie
            {
                Id = user.Id,
                Title = user.Title,
                Producers = user.Producers,
                Studio = user.Studio,
                Winner = user.Winner,
                InsertDate = user.InsertDate,
                UpdateDate = user.UpdateDate,
                UserInsert = user.UserInsert,
                UserUpdate = user.UserUpdate
            };
        }
        #endregion Movie
    }
}

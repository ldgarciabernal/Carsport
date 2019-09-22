namespace Carsport.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Models;
    using Interfaces;
    using SQLite;
    using Xamarin.Forms;

    public class DataService
    {
        private SQLiteAsyncConnection connection;

        public DataService()
        {
            this.OpenOrCreateDB();
        }

        private async Task OpenOrCreateDB()
        {
            var databasePath = DependencyService.Get<IPathService>().GetDatabasePath();
            this.connection = new SQLiteAsyncConnection(databasePath);
            await connection.CreateTableAsync<Station>().ConfigureAwait(false);
        }

        public async Task Insert<T>(T model)
        {
            await this.connection.InsertAsync(model);
        }

        public async Task Insert<T>(List<T> models)
        {
            await this.connection.InsertAllAsync(models);
        }

        public async Task Update<T>(T model)
        {
            await this.connection.UpdateAsync(model);
        }

        public async Task Update<T>(List<T> models)
        {
            await this.connection.UpdateAllAsync(models);
        }

        public async Task Delete<T>(T model)
        {
            await this.connection.DeleteAsync(model);
        }

        public async Task<List<Station>> GetAllStations()
        {
            var query = await this.connection.QueryAsync<Station>("select * from [Station]");
            var array = query.ToArray();
            var list = array.Select(p => new Station
            {
                Id = p.Id,
                Name = p.Name,
                CoreId = p.CoreId,
            }).OrderBy(p => p.Name).ToList();
            return list;
        }

        public async Task DeleteAllStations()
        {
            var query = await this.connection.QueryAsync<Station>("delete from [Station]");
        }

    }
}
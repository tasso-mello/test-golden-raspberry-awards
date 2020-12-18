namespace data.raspberry.awards.Infrastructure
{
    using data.raspberry.awards.Context;
    using data.raspberry.awards.Entities.Base;
    using Microsoft.EntityFrameworkCore;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;

	public class Repository<T> : IRepository<T> where T : BaseEntity
    {
		#region Properties

		private readonly RaspberryContext dataContext;
		private readonly DbSet<T> dbSet;

		#endregion

		protected Repository(RaspberryContext dbContext)
		{
			dataContext = dbContext;
			dbSet = dataContext.Set<T>();
		}

		#region Implementation

		public virtual void Add(T entity, long idUser)
		{
			dbSet.Add(entity);
			OnBeforeSaving(idUser);
			SaveChanges();
		}

		public virtual void AddAll(IEnumerable<T> entities, long idUser)
		{
			dbSet.AddRange(entities);
			OnBeforeSaving(idUser);
			SaveChanges();
		}

		public virtual void Update(T entity, long idUser)
		{
			dbSet.Attach(entity);
			dataContext.Entry(entity).State = EntityState.Modified;
			OnBeforeSaving(idUser);
			SaveChanges();
		}

		public virtual void UpdateAll(IEnumerable<T> entities, long idUser)
		{
			dbSet.AttachRange(entities);
			foreach (var entity in entities)
				dataContext.Entry(entity).State = EntityState.Modified;
			OnBeforeSaving(idUser);
			SaveChanges();
		}

		public virtual void Delete(T entity)
		{
			dbSet.Remove(entity);
			dataContext.Entry(entity).State = EntityState.Deleted;
			SaveChanges();
		}

		public virtual void Delete(Expression<Func<T, bool>> where)
		{
			IEnumerable<T> objects = dbSet.Where<T>(where).AsEnumerable();
			foreach (T obj in objects)
			{
				Delete(obj);
			}

			SaveChanges();
		}

		public bool Exists(Expression<Func<T, bool>> where)
		{
			return dbSet.Any(where);
		}

		public virtual T GetById(long id)
		{
			return dbSet.Find(id);
		}

		public virtual IEnumerable<T> GetAll(List<string> includes = null)
		{
			var query = dbSet.Select(c => c);
			if (includes != null)
			{
				includes.ForEach(include => query = query.Include(include));
			}
			return query.ToList();
		}

		public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where, List<string> includes = null)
		{
			var query = dbSet.Select(c => c).Where(where);
			if (includes != null)
			{
				includes.ForEach(include => query = query.Include(include));
			}
			return query.ToList();
		}

		public T Get(Expression<Func<T, bool>> where, List<string> includes = null)
		{
			var query = dbSet.Select(c => c).Where(where);
			if (includes != null)
			{
				includes.ForEach(include => query = query.Include(include));
			}

			return query.FirstOrDefault<T>();
		}

		public void SaveChanges()
		{
			dataContext.SaveChanges();
		}

		#endregion

		private void OnBeforeSaving(long idUser)
		{
			var entries = dataContext.ChangeTracker.Entries();
			foreach (var entry in entries)
			{
				if (entry.Entity is BaseEntity trackable)
				{
					var now = DateTime.Now;
					switch (entry.State)
					{
						case EntityState.Modified:
							trackable.UpdateDate = now;
							trackable.UserUpdate = idUser;
							break;
						case EntityState.Added:
							trackable.InsertDate = now;
							trackable.UserInsert = idUser;
							break;
					}
				}
			}
		}
	}
}

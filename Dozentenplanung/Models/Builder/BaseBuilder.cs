using System;
using System.Collections.Generic;

namespace Dozentenplanung.Models
{
    public abstract class BaseBuilder
    {
        public BaseObject Object { get; set; }
        protected ApplicationDbContext DatabaseContext { get; set; }
        public List<BuilderError> Errors { get; set; }

        public BaseBuilder(ApplicationDbContext aContext)
        {
            this.DatabaseContext = aContext;
        }
        public BaseBuilder(ApplicationDbContext aContext, BaseObject anObject) : this(aContext){
            this.Object = anObject;
        }

        public bool isNew() {
            return this.Object == null;
        }
        protected void saveChanges() {
            this.DatabaseContext.SaveChanges();
        }
        public bool hasError() {
            return this.Errors.Count > 1;
        }

        public abstract void save();
    }

    public class BuilderError {
        public int Number { get; set; }
        public string Text { get; set; }

        public BuilderError(int aNumber, string aText) {
            this.Number = aNumber;
            this.Text = aText;
        }
    }
}

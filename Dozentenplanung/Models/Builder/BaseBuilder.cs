using System;
using System.Collections.Generic;

namespace Dozentenplanung.Models
{
    public abstract class BaseBuilder
    {
        public BaseObject Object { get; set; }
        protected ApplicationDbContext DatabaseContext { get; set; }

        public BaseBuilder(ApplicationDbContext aContext)
        {
            this.DatabaseContext = aContext;
        }
        public BaseBuilder(ApplicationDbContext aContext, BaseObject anObject) : this(aContext){
            this.Object = anObject;
        }

        public bool isNew() {
            //Answer whether the receiver creates a new object
            return this.Object == null;
        }
        public void Save()
        {
            //Save the object
            this.Save(true);
        }
        public void Save(bool aCommitBoolean) {
            //Save the object. Commit the changes according to the given parameter
            BaseObject theObject = this.saveChanges();
            if (aCommitBoolean) {
                this.DatabaseContext.SaveChanges();
            }
            this.Object = theObject;
        }

        protected abstract BaseObject saveChanges();
    }
}

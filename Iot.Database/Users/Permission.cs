using Iot.Database.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Database.Users
{
    public class Permission
    {
        public Guid Id { get; set; }
        [TableForeignKey(typeof(User),TableConstraint.Cascading, RelationshipOneTo.Many)]
        public Guid UserId { get; set; }
        public string Resource { get; set; } = "*";// e.g., table name, file name
        public ActionFlags Actions { get; set; } = ActionFlags.None; // e.g., read, write, delete

        public Permission() { }
        public Permission(Guid userId, string resource, ActionFlags actions)
        {
            UserId = userId;
            Resource = resource;
            Actions = actions;
        }
        public static class Default
        {
            public static Permission Admin
            {
                get
                {
                    var permission = new Permission { Resource = "*", Actions = ActionFlags.FullControl };
                    return permission;
                }
            }

            public static Permission Editor
            {
                get
                {
                    var permission = new Permission { Resource = "*", Actions = ActionFlags.Read | ActionFlags.Write | ActionFlags.Update };
                    return permission;
                }
            }

            public static Permission Viewer
            {
                get
                {
                    var permission = new Permission { Resource = "*", Actions = ActionFlags.Read };
                    return permission;
                }
            }

            public static Permission Operator
            {
                get
                {
                    var permission = new Permission { Resource = "*", Actions = ActionFlags.Read | ActionFlags.Execute };
                    return permission;
                }
            }

            public static Permission Guest
            {
                get
                {
                    var permission = new Permission { Resource = "*", Actions = ActionFlags.Read };
                    return permission;
                }
            }


        }
    }
}

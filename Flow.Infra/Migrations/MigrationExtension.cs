﻿using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flow.Infra.Migrations
{
    public static class MigrationExtension
    {
        public static void DatabaseMigrate(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
            runner.ListMigrations();
            runner.MigrateUp();
        }
    }
}

module.exports = {
src: {
        styles: 'styles.scss',
        client: 'app.ts'
    },
path: {
        styles: './Content/Src/Styles/',
        dist: './wwwroot/Dist/',
        scripts: './Content/src/Scripts/'
    },
asset: {
        manifest: 'assets.json',
        vendor: 'vendor.js',
        client: 'client.js',
        style: 'style.css'
    },
uglify: {
        compress: {
        dead_code: true,
        unused: true,
        drop_console: true
      }
   },
hash: {
      enable: {
         hashLength: 8,
         template: '<%= name %>-<%= hash %><%= ext %>'
      },
      disable: {
         template: '<%= name %><%= ext %>'
      }
   }
}
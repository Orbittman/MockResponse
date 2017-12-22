module.exports = {
styles: {
        path: './Content/Styles/',
        file: 'styles.scss'
    },
path: {
        dist: './wwwroot/Dist/',
        src: './Content/src/'
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
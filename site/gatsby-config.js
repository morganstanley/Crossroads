module.exports = {
  siteMetadata: {
    title: `Crossroads`,
    description: `Crossroads is a dotnet core commandline tool packager for developers.`,
    siteUrl: 'https://morganstanley.github.io/Crossroads',
    documentationUrl: false,
    //  documentationUrl: url-of.documentation.site,
  },
  pathPrefix: `/Crossroads`, // include subdirectory
  plugins: [
    {
      resolve: '@morganstanley/gatsby-theme-ms-gh-pages',
      options: {
        indexContent: 'content',
      },
    },
  ],
};

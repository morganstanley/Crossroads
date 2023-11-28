const { plugins } = require('./src/config/base-gatsby-plugins');

module.exports = {
  siteMetadata: {
    title: `Morgan Stanley Open Source Software`,
    description: `Morgan Stanley Open Source Software`,
    siteUrl: 'http://opensource.morganstanley.com/crossroads',
    documentationUrl: false,
    //  documentationUrl: url-of.documentation.site,
  },
  pathPrefix: `/Crossroads`, // include subdirectory
  plugins,
};

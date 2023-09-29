import React from 'react';
import { Link, graphql } from 'gatsby';

import Hero from '../../components/hero';
import Layout from '../../components/layout';
import Seo from '../../components/seo';

const NewsIndex = ({ data, location }) => {
  const news = data.allMdx.nodes;

  return (
    <Layout data={data} location={location}>
      <div className="main news-main">
        <Hero title="News">
          <p>
            Jelly beans jelly beans tootsie roll caramels icing. Bear claw sweet
            roll cake lemon drops halvah. Topping tootsie roll tiramisu caramels
            bear claw topping dessert dessert chocolate cake. Chocolate bar tart
            soufflé jujubes muffin carrot cake bonbon tootsie roll tiramisu.
            Chupa chups sweet roll gingerbread jelly-o marshmallow brownie
            chocolate bar tootsie roll topping.
          </p>
        </Hero>
        <Seo title="News" />
        {news.map((node) => {
          const title = node.frontmatter.title;
          const date = new Date(node.frontmatter.date);
          return (
            <article className="content news-content" key={node.fields.slug}>
              <div className="eyebrow">{date.toLocaleDateString()}</div>
              <h3>
                <Link to={node.fields.slug}>{title}</Link>
              </h3>
              <section>{node.excerpt}</section>
            </article>
          );
        })}
      </div>
    </Layout>
  );
};

export default NewsIndex;

export const pageQuery = graphql`
  query {
    site {
      siteMetadata {
        title
        documentationUrl
      }
    }
    allMdx(
      filter: { internal: { contentFilePath: { regex: "/news/" } } }
      sort: [{ frontmatter: { date: DESC } }]
    ) {
      nodes {
        excerpt
        frontmatter {
          date
          title
        }
        internal {
          contentFilePath
        }
        fields {
          slug
        }
      }
    }
  }
`;

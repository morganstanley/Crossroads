import React from 'react';
import { Link, graphql } from 'gatsby';

import Layout from '../../components/layout';
import Seo from '../../components/seo';
import Hero from '../../components/hero';

const DocumentationIndex = ({ data, location }) => {
  const docs = data.allMdx.nodes;

  return (
    <Layout data={data} location={location}>
      <div className="main docs-main">
        <Hero title="Documentation">
          <p>
            Jelly beans jelly beans tootsie roll caramels icing. Bear claw sweet
            roll cake lemon drops halvah. Topping tootsie roll tiramisu caramels
            bear claw topping dessert dessert chocolate cake. Chocolate bar tart
            soufflé jujubes muffin carrot cake bonbon tootsie roll tiramisu.
            Chupa chups sweet roll gingerbread jelly-o marshmallow brownie
            chocolate bar tootsie roll topping.
          </p>
        </Hero>
        {docs.map((node) => {
          const title = node.frontmatter.title;
          const toc = node.tableOfContents.items;
          return (
            <article className="content" key={node.fields.slug}>
              <h3>
                <Link to={node.fields.slug}>{title}</Link>
              </h3>
              <ul>
                {toc &&
                  toc.map((item) => (
                    <li>
                      <Link to={`${node.fields.slug}${item.url}`}>
                        {item.title}
                      </Link>
                    </li>
                  ))}
              </ul>
            </article>
          );
        })}
        <Seo title="Documentation" />
      </div>
    </Layout>
  );
};

export default DocumentationIndex;

export const pageQuery = graphql`
  query {
    site {
      siteMetadata {
        title
        documentationUrl
      }
    }
    allMdx(
      filter: { internal: { contentFilePath: { regex: "/documentation/" } } }
      sort: [{ frontmatter: { order: ASC } }]
    ) {
      nodes {
        id
        tableOfContents
        frontmatter {
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

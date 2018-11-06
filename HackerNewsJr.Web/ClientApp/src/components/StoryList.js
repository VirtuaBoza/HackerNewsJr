import React, { Component } from 'react';
import PropTypes from 'prop-types';
import Highligher from 'react-highlight-words';

const StoryList = ({ stories, searchString, page, onMoreClick }) => {
  return (
    <table
      border="0"
      cellPadding="0"
      cellSpacing="0"
      className="itemlist"
      style={{ textAlign: 'left' }}
    >
      <tbody>
        {stories.length === 0 ? (
          <tr>
            <td>No stories match your search.</td>
          </tr>
        ) : (
          stories
            .map((story, index) => (
              <React.Fragment key={story.id}>
                <tr className="athing">
                  <td align="right" valign="top" className="title">
                    <span className="rank">{index + 1}.</span>
                  </td>
                  <td className="title">
                    <a href={story.url} className="storylink">
                      <Highligher
                        highlightClassName="highlighted"
                        searchWords={[searchString]}
                        textToHighlight={story.title}
                      />
                    </a>
                  </td>
                </tr>
                <tr>
                  <td />
                  <td className="subtext">by {story.by}</td>
                </tr>
                <tr className="spacer" style={{ height: '5px' }} />
              </React.Fragment>
            ))
            .filter(
              (story, index) => index >= 30 * (page - 1) && index < 30 * page,
            )
        )}

        <tr className="morespace" style={{ height: '10px' }} />
        <tr>
          <td />
          <td className="title">
            {stories.length > 30 * page && (
              <span
                className="morelink"
                style={{ cursor: 'pointer' }}
                onClick={onMoreClick}
              >
                More
              </span>
            )}
          </td>
        </tr>
      </tbody>
    </table>
  );
};

StoryList.propTypes = {
  stories: PropTypes.array.isRequired,
  searchString: PropTypes.string.isRequired,
  page: PropTypes.number.isRequired,
  onMoreClick: PropTypes.func.isRequired,
};

export default StoryList;

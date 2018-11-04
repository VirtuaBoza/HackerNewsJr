import React, { Component } from 'react';
import PropTypes from 'prop-types';
import Highligher from 'react-highlight-words';

class StoryList extends Component {
  constructor(props) {
    super(props);

    this.state = {
      page: 1,
    };

    this.handleMoreClick = this.handleMoreClick.bind(this);
  }

  handleMoreClick() {
    const page = this.state.page + 1;
    this.setState({ page });
  }

  render() {
    const { stories, searchString } = this.props;

    return (
      <table
        border="0"
        cellPadding="0"
        cellSpacing="0"
        className="itemlist"
        style={{ textAlign: 'left' }}
      >
        <tbody>
          {stories
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
              (story, index) =>
                index >= 30 * (this.state.page - 1) &&
                index < 30 * this.state.page,
            )}
          <tr className="morespace" style={{ height: '10px' }} />
          <tr>
            <td />
            <td className="title">
              {stories.length > 30 * this.state.page && (
                <span
                  className="morelink"
                  style={{ cursor: 'pointer' }}
                  onClick={this.handleMoreClick}
                >
                  More
                </span>
              )}
            </td>
          </tr>
        </tbody>
      </table>
    );
  }
}

StoryList.propTypes = {
  stories: PropTypes.array.isRequired,
  searchString: PropTypes.string.isRequired,
};

export default StoryList;

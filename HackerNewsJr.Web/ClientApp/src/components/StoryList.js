import React, { Component } from 'react';
import Highligher from 'react-highlight-words';

class StoryList extends Component {
  constructor(props) {
    super(props);

    this.state = {
      set: 0,
    };

    this.handleMoreClick = this.handleMoreClick.bind(this);
  }

  handleMoreClick() {
    const set = this.state.set + 1;
    this.setState({ set });
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
                  <td className="subtext">
                    <span className="score">{story.score} points</span> by{' '}
                    {story.by}
                  </td>
                </tr>
                <tr className="spacer" style={{ height: '5px' }} />
              </React.Fragment>
            ))
            .filter(
              (story, index) =>
                index >= 30 * this.state.set &&
                index < 30 * (this.state.set + 1),
            )}
          <tr className="morespace" style={{ height: '10px' }} />
          <tr>
            <td />
            <td className="title">
              {stories.length > 30 * (this.state.set + 1) && (
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

export default StoryList;

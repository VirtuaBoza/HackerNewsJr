import React from 'react';

const Footer = props => {
  return (
    <React.Fragment>
      <br />
      <table width="100%" cellSpacing="0" cellPadding="1">
        <tbody>
          <tr>
            <td bgcolor="#ff6600" />
          </tr>
        </tbody>
      </table>
      <br />
      <center>
        <span className="yclinks">
          <a href="https://github.com/HackerNews/API">API</a>
        </span>
        <br />
        <br />
        <form>
          Search:
          <input
            type="text"
            value={props.searchString}
            size="17"
            autoCorrect="off"
            spellCheck="false"
            autoCapitalize="off"
            autoComplete="false"
            onChange={props.onChange}
          />
        </form>
        <br />
      </center>
    </React.Fragment>
  );
};

export default Footer;

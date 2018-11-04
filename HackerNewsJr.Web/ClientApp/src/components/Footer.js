import React from 'react';
import PropTypes from 'prop-types';

const Footer = ({ searchString, onChange }) => {
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
            value={searchString}
            size="17"
            autoCorrect="off"
            spellCheck="false"
            autoCapitalize="off"
            autoComplete="false"
            onChange={onChange}
          />
        </form>
        <br />
      </center>
    </React.Fragment>
  );
};

Footer.propTypes = {
  searchString: PropTypes.string.isRequired,
  onChange: PropTypes.func.isRequired,
};

export default Footer;

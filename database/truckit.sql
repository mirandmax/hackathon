-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: localhost
-- Generation Time: Nov 11, 2023 at 12:11 PM
-- Server version: 10.4.28-MariaDB
-- PHP Version: 8.2.4

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `truckit`
--

-- --------------------------------------------------------

--
-- Table structure for table `companies`
--

CREATE TABLE `companies` (
  `cid` int(11) NOT NULL,
  `cname` varchar(100) DEFAULT NULL,
  `cnation` varchar(50) DEFAULT NULL,
  `cplz` int(10) DEFAULT NULL,
  `ccity` varchar(50) DEFAULT NULL,
  `cstreet` varchar(100) DEFAULT NULL,
  `chnr` varchar(10) DEFAULT NULL,
  `cphone` varchar(30) NOT NULL,
  `cfax` varchar(30) DEFAULT NULL,
  `cemail` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_german1_ci;

--
-- Dumping data for table `companies`
--

INSERT INTO `companies` (`cid`, `cname`, `cnation`, `cplz`, `ccity`, `cstreet`, `chnr`, `cphone`, `cfax`, `cemail`) VALUES
(1, 'test', 'a', 2, 'a', 'a', 'a', 'a', 'a', 'a'),
(2, NULL, NULL, NULL, NULL, NULL, NULL, '+39 123 345 5678', NULL, 'aaa');

-- --------------------------------------------------------

--
-- Table structure for table `trucklocations`
--

CREATE TABLE `trucklocations` (
  `tid` int(11) NOT NULL,
  `tdate` date NOT NULL DEFAULT current_timestamp(),
  `tlat` double(9,6) NOT NULL,
  `tlon` double(9,6) NOT NULL,
  `uid` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_german1_ci;

--
-- Dumping data for table `trucklocations`
--

INSERT INTO `trucklocations` (`tid`, `tdate`, `tlat`, `tlon`, `uid`) VALUES
(1, '2020-01-01', 50.000000, 10.000000, 15),
(1, '2023-11-11', 50.000000, 10.000000, 15),
(2, '2023-11-01', 51.000000, 10.000000, 15),
(2, '2023-11-10', 50.000210, 10.009300, 17),
(2, '2023-11-11', 50.000139, 10.009760, 16);

-- --------------------------------------------------------

--
-- Table structure for table `trucks`
--

CREATE TABLE `trucks` (
  `tid` int(11) NOT NULL,
  `tplate` varchar(15) NOT NULL,
  `cid` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_german1_ci;

--
-- Dumping data for table `trucks`
--

INSERT INTO `trucks` (`tid`, `tplate`, `cid`) VALUES
(1, 'AA111AA', 2),
(2, 'fdnskalfndsla', 1);

-- --------------------------------------------------------

--
-- Table structure for table `user`
--

CREATE TABLE `user` (
  `uid` int(11) NOT NULL,
  `uname` varchar(30) NOT NULL,
  `passwd` varchar(256) NOT NULL,
  `cid` int(11) DEFAULT NULL,
  `ucredits` int(11) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_german1_ci;

--
-- Dumping data for table `user`
--

INSERT INTO `user` (`uid`, `uname`, `passwd`, `cid`, `ucredits`) VALUES
(15, 'taschler', '15ddd9fff661a0bccb3572fe77903eea', 1, 0),
(16, 'mirandola', '92b7008f8d4950d745b57e3a6085829f', 2, 0),
(17, 'wachtler', '97a9523a99f7d1e0a361257d988f01aa', NULL, 0);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `companies`
--
ALTER TABLE `companies`
  ADD PRIMARY KEY (`cid`);

--
-- Indexes for table `trucklocations`
--
ALTER TABLE `trucklocations`
  ADD PRIMARY KEY (`tid`,`tdate`),
  ADD KEY `uid` (`uid`);

--
-- Indexes for table `trucks`
--
ALTER TABLE `trucks`
  ADD PRIMARY KEY (`tid`),
  ADD KEY `cid` (`cid`);

--
-- Indexes for table `user`
--
ALTER TABLE `user`
  ADD PRIMARY KEY (`uid`),
  ADD UNIQUE KEY `uname` (`uname`),
  ADD KEY `cid` (`cid`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `companies`
--
ALTER TABLE `companies`
  MODIFY `cid` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `trucklocations`
--
ALTER TABLE `trucklocations`
  MODIFY `tid` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `trucks`
--
ALTER TABLE `trucks`
  MODIFY `tid` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `user`
--
ALTER TABLE `user`
  MODIFY `uid` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=18;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `trucklocations`
--
ALTER TABLE `trucklocations`
  ADD CONSTRAINT `trucklocations_ibfk_1` FOREIGN KEY (`uid`) REFERENCES `user` (`uid`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `trucks`
--
ALTER TABLE `trucks`
  ADD CONSTRAINT `trucks_ibfk_1` FOREIGN KEY (`cid`) REFERENCES `companies` (`cid`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `user`
--
ALTER TABLE `user`
  ADD CONSTRAINT `user_ibfk_1` FOREIGN KEY (`cid`) REFERENCES `companies` (`cid`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;

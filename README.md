# GPU and GPU performance test

Small project for comparing performance of a heavy load parallelized task running in GPU vs CPU.
This simple test will prepare an array of data in first place and then will launch multi-thread operations on that data to measure the performance difference between this two devices.
Data preparation will take long as it has to allocate memory.

## Requirements
1. Nvidia video card compatible with CUDA
2. Visual Studio 2015/2017
3. Nvidia CUDA 8.0, 9.0, 9.1. or 9.2 ([here](https://developer.nvidia.com/cuda-toolkit-archive))
4. Hybridizer Essentials for Visual Studio ([here](https://marketplace.visualstudio.com/items?itemName=altimesh.AltimeshHybridizerExtensionEssentials)) and a license (which you can request for free from the visual studio menu once installed).


- - - -
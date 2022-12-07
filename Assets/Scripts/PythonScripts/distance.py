import csv
import os

import bct
import numpy as np
# Press ⌃R to execute it or replace it with your code.
# Press Double ⇧ to search everywhere for classes, files, tool windows, actions, and settings.
import pandas as pd
# importing the data
from numpy import ndarray

#for standAlone running
# project_path = '../../Resources/'
# res_path = project_path

#for Unity running
project_path = os.getcwd()+'\\externalFiles\\'
res_path = os.getcwd()+'\\Assets\\Resources\\'

df_region_color = pd.read_csv(f'{project_path}HCP-MMP1_RegionColor.csv', skipfooter=19, engine='python')
df_region_list = pd.read_csv(f'{project_path}HCP-MMP1_UniqueRegionList.csv')
df_main = pd.read_csv(f'{project_path}hcpmmp1.csv', skipfooter=19, header=None, engine='python')

# converting to ndarray
ndarray_sub = np.asarray(list(df_main.get(0)))
ndarray_main = np.resize(np.asarray(dtype=float, a=ndarray_sub[0].split(' ')), (1, 360))
# converting values to float and appending to the main ndarray + removing rows 360-379
for x in range(1, ndarray_sub.size):
    # removing columns 360-379
    tmp: ndarray = np.resize(np.asarray(dtype=float, a=ndarray_sub[x].split(' ')), (1, 360))
    # print(tmp)
    ndarray_main = np.append(ndarray_main, tmp, axis=0)

# creating the binary connection graph
# ndarray_main_bin = np.logical_or(ndarray_main,ndarray_main)
ndarray_main_bin = bct.binarize(ndarray_main, True)
ndarray_main_with_distance = bct.invert(ndarray_main, True)

ndarray_main_threshold = bct.threshold_proportional(ndarray_main, p=0.1, copy=True)
ndarray_main_threshold_bin = bct.binarize(ndarray_main_threshold, True)
ndarray_main_threshold_with_distance = bct.invert(ndarray_main_threshold, True)

print(ndarray_main)

print(ndarray_main_bin)
print(ndarray_main_with_distance)

print(ndarray_main_threshold)
print(ndarray_main_threshold_bin)
print(ndarray_main_threshold_with_distance)


binary_shortest_paths = bct.distance_bin(ndarray_main_threshold_bin)
weighted_shortest_paths = bct.distance_wei(ndarray_main_threshold_with_distance)
print(binary_shortest_paths)
print(weighted_shortest_paths)


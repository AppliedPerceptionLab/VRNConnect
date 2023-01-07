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

threshold = 0.05

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

ndarray_main_threshold_prop = bct.threshold_proportional(ndarray_main, p=threshold, copy=True)
ndarray_main_threshold_abs = bct.threshold_absolute(ndarray_main, thr=threshold * ndarray_main.max(initial=0), copy=True)
ndarray_main_threshold_abs_bin = bct.binarize(ndarray_main_threshold_abs, True)
ndarray_main_threshold_prop_bin = bct.binarize(ndarray_main_threshold_prop, True)
ndarray_main_threshold_abs_with_distance = bct.invert(ndarray_main_threshold_abs, True)
ndarray_main_threshold_prop_with_distance = bct.invert(ndarray_main_threshold_prop, True)

print(ndarray_main)

print(ndarray_main_bin)
print(ndarray_main_with_distance)

print(ndarray_main_threshold_abs)
ndarray_main_threshold_abs_FileName = 'ndarray_main_threshold_abs'
pd.DataFrame.from_records(ndarray_main_threshold_abs).to_json(f'{res_path}{ndarray_main_threshold_abs_FileName}.json')
pd.DataFrame.from_records(ndarray_main_threshold_abs).to_csv(f'{res_path}{ndarray_main_threshold_abs_FileName}.csv')
pd.DataFrame.from_records(ndarray_main_threshold_abs).to_csv(f'{res_path}{ndarray_main_threshold_abs_FileName}.txt')

print(ndarray_main_threshold_prop)
ndarray_main_threshold_prop_FileName = 'ndarray_main_threshold_prop'
pd.DataFrame.from_records(ndarray_main_threshold_prop).to_json(f'{res_path}{ndarray_main_threshold_prop_FileName}.json')
pd.DataFrame.from_records(ndarray_main_threshold_prop).to_csv(f'{res_path}{ndarray_main_threshold_prop_FileName}.csv')
pd.DataFrame.from_records(ndarray_main_threshold_prop).to_csv(f'{res_path}{ndarray_main_threshold_prop_FileName}.txt')


print(ndarray_main_threshold_abs_bin)
print(ndarray_main_threshold_prop_bin)
print(ndarray_main_threshold_abs_with_distance)
print(ndarray_main_threshold_prop_with_distance)


binary_shortest_paths = bct.distance_bin(ndarray_main_threshold_abs_bin)
# binary_shortest_paths = bct.distance_bin(ndarray_main_threshold_prop_bin)

'''
Returns
    -------
    D : NxN np.ndarray
        distance (shortest weighted path) matrix
    B : NxN np.ndarray
        matrix of number of edges in shortest weighted path
        
        Algorithm: Dijkstra's algorithm.
'''
weighted_shortest_paths = bct.distance_wei(ndarray_main_threshold_abs_with_distance)
# weighted_shortest_paths = bct.distance_wei(ndarray_main_threshold_prop_with_distance)
print(binary_shortest_paths)

print(weighted_shortest_paths)
weighted_shortest_paths_FileName = 'weighted_shortest_paths'
pd.DataFrame.from_records(weighted_shortest_paths).to_json(f'{res_path}{weighted_shortest_paths_FileName}.json')
pd.DataFrame.from_records(weighted_shortest_paths).to_csv(f'{res_path}{weighted_shortest_paths_FileName}.csv')
pd.DataFrame.from_records(weighted_shortest_paths).to_csv(f'{res_path}{weighted_shortest_paths_FileName}.txt')

'''
Returns
    -------
    R : NxN np.ndarray
        binary reachability matrix
    D : NxN np.ndarray
        distance matrix
        
        Algorithm: BFS algorithm.
'''

breadth_dist = bct.breadthdist(ndarray_main_threshold_abs_with_distance)
print(breadth_dist)
bfsFileName = 'breadth_distance'
pd.DataFrame.from_records(breadth_dist[1]).to_json(f'{res_path}{bfsFileName}.json')
pd.DataFrame.from_records(breadth_dist[1]).to_csv(f'{res_path}{bfsFileName}.csv')
pd.DataFrame.from_records(breadth_dist[1]).to_csv(f'{res_path}{bfsFileName}.txt')

floyd_shortest_paths = bct.distance_wei_floyd(ndarray_main_threshold_abs_bin)
print(floyd_shortest_paths)
floydFileName = 'floyd_shortest_paths'
pd.DataFrame.from_records(floyd_shortest_paths[0]).to_csv(f'{res_path}{floydFileName}_spl.csv')
pd.DataFrame.from_records(floyd_shortest_paths[1]).to_csv(f'{res_path}{floydFileName}_hops.csv')
pd.DataFrame.from_records(floyd_shortest_paths[2]).to_csv(f'{res_path}{floydFileName}_pmat.csv')

